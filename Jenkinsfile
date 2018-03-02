pipeline {
    agent any
    triggers {
        pollSCM 'H/10 * * * *'
    }
    options { skipDefaultCheckout() }
    stages {
        stage('Checkout') {
            steps {
                checkout([
                    $class: 'GitSCM', branches: [[name: '*']],
                    userRemoteConfigs: [[url: 'https://github.com/GreenSense/SoilMoistureSensorCalibratedSerial.git',credentialsId:'GitHub']]
                ])
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
