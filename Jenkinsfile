pipeline {
    agent any
    triggers {
        pollSCM 'H/30 * * * *'
    }
    stages {
        stage('Prepare') {
            steps {
                sh 'echo "Skipping prepare.sh script call to speed up tests. Prerequisites should already be installed." # sh prepare.sh'
            }
        }
        stage('Init') {
            steps {
                sh 'sh init.sh'
            }
        }
        stage('Build') {
            steps {
                sh 'sh build-all.sh'
            }
        }
        stage('Publish') {
            steps {
              publishHTML target: [
                  allowMissing: false,
                  alwaysLinkToLastBuild: false,
                  keepAll: true,
                  reportDir: 'report',
                  reportFiles: 'index.html',
                  reportName: 'Test Report'
                ]
            }
        }
    }
    post {
        always {
            cleanWs()
        }
    }
}
