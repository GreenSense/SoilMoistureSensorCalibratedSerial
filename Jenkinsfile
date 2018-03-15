pipeline {
    agent any
    triggers {
        pollSCM 'H/30 * * * *'
    }
    stages {
        
        stage('Graduate') {
            steps {
                checkout scm
                sh 'git branch'
                sh 'git fetch'
                sh 'git checkout $BRANCH_NAME'
                sh 'sh graduate.sh'
            }
        }
    }
    post {
        always {
            cleanWs()
        }
    }
}
