pipeline {
    agent any
    triggers {
        pollSCM 'H/10 * * * *'
    }
    options {
        skipDefaultCheckout()      // Don't checkout automatically
    }
    stages {
        stage('Checkout') {
            steps {
              echo 'Pulling...' + env.BRANCH_NAME
              checkout scm
            }
        }
        stage('Prepare') {
            steps {
                sh 'echo "Skipping prepare.sh script call to speed up tests. Prerequisites should already be installed." # sh prepare.sh'
            }
        }
        stage('Graduate') {
            steps {
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
