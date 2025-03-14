pipeline {
    agent any

    environment {
        GITHUB_CREDENTIALS = credentials('github-credentials')
    }

    triggers {
        pollSCM('H/2 * * * *') 
    }

    stages {
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
                        
                        // Obtém o número do PR correspondente ao branch atual
                        def PR_NUMBER = bat(
                            script: '''
                            @echo off
                            setlocal enabledelayedexpansion
                            curl -s -H "Authorization: Bearer %GITHUB_TOKEN%" ^
                            -H "Accept: application/vnd.github.v3+json" ^
                            "https://api.github.com/repos/Matheus-Bernardo/Exchange-or-Loans/pulls?state=open" | jq -r ".[] | select(.head.ref == 'Projetps') | .number"
                            ''',
                            returnStdout: true
                        ).trim()

                        if (!PR_NUMBER?.isInteger()) {
                            echo "Nenhum PR válido encontrado para a branch Projetps. Pulando merge."
                            return
                        }

                        echo "Tentando mesclar PR #${PR_NUMBER}"

                        def mergeStatus = bat(
                            script: '''
                            @echo off
                            curl -s -H "Authorization: Bearer %GITHUB_TOKEN%" ^
                            -H "Accept: application/vnd.github.v3+json" ^
                            "https://api.github.com/repos/Matheus-Bernardo/Exchange-or-Loans/pulls/%PR_NUMBER%" | jq -r ".mergeable"
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
                        } else {
                            echo "PR não é mesclável. Pulando merge."
                        }
                    }
                }
            }
        }
    }
}
