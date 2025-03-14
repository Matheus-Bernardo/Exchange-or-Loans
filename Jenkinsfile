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
            when {
                expression { return env.CHANGE_ID != null } 
            }
            steps {
                script {
                    def PR_NUMBER = env.CHANGE_ID
                    echo "Attempting to merge PR #${PR_NUMBER}"
                    
                    def mergeStatus = bat(
                        script: """
                        curl -s -H "Authorization: Bearer ${GITHUB_TOKEN}" \
                        -H "Accept: application/vnd.github.v3+json" \
                        "https://api.github.com/repos/Matheus-Bernardo/Exchange-or-Loans/pulls/${PR_NUMBER}" | jq -r '.mergeable'
                        """,
                        returnStdout: true
                    ).trim()

                     if (mergeStatus == "true") {
                        echo "PR is mergeable. Proceeding..."
                        bat """
                        curl -X PUT -H "Authorization: Bearer ${GITHUB_TOKEN}" \
                        -H "Accept: application/vnd.github.v3+json" \
                        -d '{ "merge_method": "squash" }' \
                        "https://api.github.com/repos/Matheus-Bernardo/Exchange-or-Loans/pulls/${PR_NUMBER}/merge"
                        """
                        echo "Merge completed successfully."
                    } else {
                        echo "PR is not mergeable. Skipping merge."
                    }
                }
            }
        }
    }
}
