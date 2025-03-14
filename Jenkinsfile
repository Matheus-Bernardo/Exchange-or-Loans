pipeline {
    agent any

    triggers {
        githubPullRequest {
            orgWhitelist(['Matheus-Bernardo'])
            allowMembersOfWhitelistedOrgsAsAdmin()
            permitAll()
        }
    }

    environment {
        GITHUB_CREDENTIALS = credentials('github-credentials') 
    }

    stages {
        stage('Checkout') {
            steps {
                checkout scm
            }
        }
        stage('Install Dependencies') {
            steps {
                sh 'dotnet restore' 
            }
        }
        stage('Run Tests') {
            steps {
                sh 'dotnet test' 
            }
        }
    }
}
