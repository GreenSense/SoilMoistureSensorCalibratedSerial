pipeline {
    agent any

    stages {
        stage('Prepare') {
            steps {
                sh 'sh prepare.sh'
            }
        }
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
        stage('Upload') {
            steps {
                sh 'sh upload.sh'
            }
        }
        stage('Test') {
            steps {
                sh 'sh test.sh'
            }
        }
    }
}
