pipeline {
    agent any
    triggers {
        pollSCM 'H/10 * * * *'
    }
    options { skipDefaultCheckout() }
    environment {
      GITUSER = credentials('GitHub')
    }
    stages {
        stage('Checkout') {
            steps {
                checkout([
                    $class: 'GitSCM', branches: [[name: '*']],
                    userRemoteConfigs: [[url: 'https://${GITUSER_USR}:${GITUSER_PSW}@github.com/GreenSense/SoilMoistureSensorCalibratedSerial.git',credentialsId:'GitHub']]
                ])
                sh 'git checkout ' + env.BRANCH_NAME
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
