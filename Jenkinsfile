pipeline {
    agent any

    stages {
        stage('Prepare') {
            steps {
                sh('prepare.sh')
            }
        }
        stage('Build') {
            steps {
                sh('build.sh')
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
