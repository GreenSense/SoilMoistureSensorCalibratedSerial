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
              sh 'git clone ' + env.GIT_URL + " -b " + env.GIT_BRANCH
            }
        }
    }
    post {
        always {
            cleanWs()
        }
    }
}
