pipeline {
    agent any
    triggers {
        pollSCM 'H/10 * * * *'
    }
    options { skipDefaultCheckout() }
    stages {
        stage('Checkout') {
            steps {
              echo 'Branch: ' + env.BRANCH_NAME
              sh 'git clone https://github.com/GreenSense/SoilMoistureSensorCalibratedSerial.git -b ' + env.BRANCH_NAME
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
