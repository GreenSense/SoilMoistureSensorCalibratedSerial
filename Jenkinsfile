pipeline {
    agent any
    triggers {
        pollSCM 'H/3 * * * *'
    }
    stages {
        stage('Init') {
            steps {
                sh 'sh init.sh'
            }
        }
        stage('Build') {
            steps {
                sh 'sh build.sh'
            }
        }
    }
    post {
        always {
            cleanWs()
        }
    }
}
