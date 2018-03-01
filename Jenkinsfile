pipeline {
    agent any
    triggers {
        pollSCM 'H/10 * * * *'
    }
    options { skipDefaultCheckout() }
    stages {
        stage('Checkout') {
            steps {
              echo 'Pulling...' + env.BRANCH_NAME
              sh 'git clone https://github.com/GreenSense/SoilMoistureSensorCalibratedSerial.git -b ' + env.BRANCH_NAME
            }
        }
    }
    post {
        always {
            cleanWs()
        }
    }
}
