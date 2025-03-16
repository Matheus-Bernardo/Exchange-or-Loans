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
                            curl -s -H "Authorization: Bearer %GITHUB_TOKEN%" ^
                            -H "Accept: application/vnd.github.v3+json" ^
                            "https://api.github.com/repos/Matheus-Bernardo/Exchange-or-Loans/pulls?state=open"
                            ''',
                            returnStdout: true
                        ).trim()

                        echo "Resposta da API do GitHub para PRs Abertos: ${response}"

                        // Extraindo dinamicamente a branch e o número do PR
                        def PR_NUMBER = bat(
                            script: '''
                            echo %response% | jq -r 'map(select(.head.ref != null)) | .[0].number'
                            ''',
                            returnStdout: true
                        ).trim()

                        def branchName = bat(
                            script: '''
                            echo %response% | jq -r 'map(select(.head.ref != null)) | .[0].head.ref'
                            ''',
                            returnStdout: true
                        ).trim()

                        if (PR_NUMBER.isEmpty()) {
                            echo "Nenhum PR válido encontrado. Pulando merge."
                            return
                        }

                        echo "Branch do PR encontrado: ${branchName}"
                        echo "Número do PR encontrado: ${PR_NUMBER}"

                        // Verificando se o PR pode ser mesclado
                        def mergeStatus = bat(
                            script: '''
                            curl -s -H "Authorization: Bearer %GITHUB_TOKEN%" ^
                            -H "Accept: application/vnd.github.v3+json" ^
                            "https://api.github.com/repos/Matheus-Bernardo/Exchange-or-Loans/pulls/%PR_NUMBER%" | jq -r ".mergeable"
                            ''',
                            returnStdout: true
                        ).trim()

                        if (mergeStatus == "true") {
                            echo "PR é mesclável. Procedendo com o merge..."
                            bat '''
                            curl -X PUT -H "Authorization: Bearer %GITHUB_TOKEN%" ^
                            -H "Accept: application/vnd.github.v3+json" ^
                            -d "{ \"merge_method\": \"squash\" }" ^
                            "https://api.github.com/repos/Matheus-Bernardo/Exchange-or-Loans/pulls/%PR_NUMBER%/merge"
                            '''
                            echo "Merge concluído com sucesso."
                        } else if (mergeStatus == "null") {
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
