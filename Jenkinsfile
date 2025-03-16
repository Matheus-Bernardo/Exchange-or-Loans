pipeline {
    agent any

    environment {
        GITHUB_CREDENTIALS = credentials('github-credentials')
    }

    stages {
        stage('Debug PATH') {
            steps {
                bat 'set'
            }
        }

        stage('Checkout') {
            steps {
                checkout([
                    $class: 'GitSCM',
                    branches: [[name: '*/Projetps']],
                    userRemoteConfigs: [[
                        url: 'https://github.com/Matheus-Bernardo/Exchange-or-Loans.git',
                        credentialsId: 'github-credentials'
                    ]]
                ])
            }
        }

        stage('Check Environment') {
            steps {
                bat '''
                @echo off
                echo Verificando cmd.exe, curl e jq...
                where cmd
                where curl
                where jq
                '''
            }
        }

        stage('Clean Workspace') {
            steps {
                bat '''
                cd ExchangeOrLoans/ExchangeOrLoans/ExchangeOrLoans.Tests
                if exist bin rmdir /s /q bin
                if exist obj rmdir /s /q obj
                cd ..
                if exist bin rmdir /s /q bin
                if exist obj rmdir /s /q obj
                '''
            }
        }

        stage('Restore Dependencies') {
            steps {
                bat 'cd ExchangeOrLoans/ExchangeOrLoans && dotnet restore'
            }
        }

        stage('Build') {
            steps {
                bat 'cd ExchangeOrLoans/ExchangeOrLoans && dotnet build'
            }
        }

        stage('Run Tests in ExchangeOrLoans.Tests') {
            steps {
                bat 'cd ExchangeOrLoans/ExchangeOrLoans/ExchangeOrLoans.Tests && dotnet test'
            }
        }

        stage('Merge PR if Tests Pass') {
            steps {
                script {
                    withCredentials([string(credentialsId: 'github-token', variable: 'GITHUB_TOKEN')]) {

                        // Obtendo PRs abertos
                        def response = bat(
                            script: '''
                            @echo off
                            curl -s -H "Authorization: Bearer %GITHUB_TOKEN%" ^
                            -H "Accept: application/vnd.github.v3+json" ^
                            "https://api.github.com/repos/Matheus-Bernardo/Exchange-or-Loans/pulls?state=open"
                            ''',
                            returnStdout: true
                        ).trim()

                        echo "Resposta da API do GitHub para PRs Abertos: ${response}"

                        // Extraindo dinamicamente a branch e o número do PR
                        def PR_INFO = bat(
                            script: '''
                            @echo off
                            echo %response% | jq -r ".[] | {branch: .head.ref, number: .number}" | jq -c "select(.branch != null and .number != null)"
                            ''',
                            returnStdout: true
                        ).trim()

                        if (PR_INFO.isEmpty()) {
                            echo "Nenhum PR válido encontrado. Pulando merge."
                            return
                        }

                        def branchName = bat(
                            script: "echo ${PR_INFO} | jq -r '.branch'",
                            returnStdout: true
                        ).trim()

                        def PR_NUMBER = bat(
                            script: "echo ${PR_INFO} | jq -r '.number'",
                            returnStdout: true
                        ).trim()

                        echo "Branch do PR encontrado: ${branchName}"
                        echo "Número do PR encontrado: ${PR_NUMBER}"

                        if (PR_NUMBER.isEmpty() || !PR_NUMBER.isNumber()) {
                            echo "Nenhum PR válido encontrado para merge. Pulando etapa."
                            return
                        }

                        // Verificando se o PR pode ser mesclado
                        def mergeStatus = bat(
                            script: '''
                            @echo off
                            curl -s -H "Authorization: Bearer %GITHUB_TOKEN%" ^
                            -H "Accept: application/vnd.github.v3+json" ^
                            "https://api.github.com/repos/Matheus-Bernardo/Exchange-or-Loans/pulls/%PR_NUMBER%" | jq -r "if .mergeable == null then \\"unknown\\" else .mergeable end"
                            ''',
                            returnStdout: true
                        ).trim()

                        if (mergeStatus == "true") {
                            echo "PR é mesclável. Procedendo com o merge..."
                            bat '''
                            @echo off
                            curl -X PUT -H "Authorization: Bearer %GITHUB_TOKEN%" ^
                            -H "Accept: application/vnd.github.v3+json" ^
                            -d "{ \\"merge_method\\": \\"squash\\" }" ^
                            "https://api.github.com/repos/Matheus-Bernardo/Exchange-or-Loans/pulls/%PR_NUMBER%/merge"
                            '''
                            echo "Merge concluído com sucesso."
                        } else if (mergeStatus == "unknown") {
                            echo "O estado de mesclagem ainda não foi determinado. Tentando novamente mais tarde."
                        } else {
                            echo "PR não é mesclável. Pulando merge."
                        }
                    }
                }
            }
        }
    }
}
