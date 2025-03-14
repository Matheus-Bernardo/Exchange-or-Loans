pipeline {
    agent any

    environment {
        GITHUB_CREDENTIALS = credentials('github-credentials-token') 
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
                        credentialsId: 'acess_token'
                    ]]
                ])
            }
        }
        stage('Run Tests') {
            steps {
                sh 'dotnet restore'  
                sh 'dotnet test'     
            }
        }
    }
}
