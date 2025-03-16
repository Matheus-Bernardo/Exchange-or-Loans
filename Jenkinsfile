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
                        bat '''
                        curl -s -H "Authorization: Bearer %GITHUB_TOKEN%" ^
                        -H "Accept: application/vnd.github.v3+json" ^
                        "https://api.github.com/repos/Matheus-Bernardo/Exchange-or-Loans/pulls?state=open" > response.json
                        '''

                        // Extraindo dinamicamente a branch e o número do PR
                        bat '''
                        for /F "delims=" %i in ('jq -r ".[] | select(.head.ref != null) | .head.ref" response.json') do set BRANCH_NAME=%i
                        '''
                        def branchName = env.BRANCH_NAME

                        bat '''
                        for /F "delims=" %i in ('jq -r ".[] | select(.head.ref != null) | .number" response.json') do set PR_NUMBER=%i
                        '''
                        def PR_NUMBER = env.PR_NUMBER

                        if (!PR_NUMBER) {
                            echo "Nenhum PR válido encontrado. Pulando merge."
                            return
                        }

                        echo "Branch do PR encontrado: ${branchName}"
                        echo "Número do PR encontrado: ${PR_NUMBER}"

                        // Aguardar atualização do GitHub
                        bat 'timeout /T 5 /NOBREAK'

                        // Verificando se o PR pode ser mesclado
                        bat '''
                        curl -s -H "Authorization: Bearer %GITHUB_TOKEN%" ^
                        -H "Accept: application/vnd.github.v3+json" ^
                        "https://api.github.com/repos/Matheus-Bernardo/Exchange-or-Loans/pulls/%PR_NUMBER%" | jq -r ".mergeable" > mergeable_status.txt
                        '''
                        def mergeStatus = readFile('mergeable_status.txt').trim()

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
                            echo "PR não é mesclável. Tentando atualizar a branch antes do merge."
                            bat '''
                            curl -X POST -H "Authorization: Bearer %GITHUB_TOKEN%" ^
                            -H "Accept: application/vnd.github.v3+json" ^
                            "https://api.github.com/repos/Matheus-Bernardo/Exchange-or-Loans/pulls/%PR_NUMBER%/update-branch"
                            '''
                            echo "Branch atualizada. Verifique o PR novamente após alguns minutos."
                        }
                    }
                }
            }
        }
    }
}
