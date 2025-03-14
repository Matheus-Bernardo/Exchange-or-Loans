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
        stage('Restore Dependencies') {
            steps {
                bat 'cd ExchangeOrLoans/ExchangeOrLoans && dotnet restore' 
            }
        }
        stage('Build') {
            steps {
                bat 'cd ExchangeOrLoans/ExchangeOrLoans && dotnet build --configuration Release --no-restore'
            }
        }
        stage('Run Tests') {
            steps {
                bat 'cd ExchangeOrLoans/ExchangeOrLoans && dotnet test --no-restore --verbosity normal'
            }
        }
    }
}
