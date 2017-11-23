pipeline {
    agent any

    stages {
        stage('Prepare') {
            steps {
                sh '# Skip prepare # sudo sh prepare.sh'
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
        stage('Test') {
            steps {
                echo 'Testing..'
            }
        }
        stage('Deploy') {
            steps {
                echo 'Deploying....'
            }
        }
    }
}
