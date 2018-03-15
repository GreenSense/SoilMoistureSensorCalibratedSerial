pipeline {
    agent any
    triggers {
        pollSCM 'H/30 * * * *'
    }
    stages {
        
        stage('Checkout') {
            steps {
                checkout scm: [$class: 'GitSCM', branches: [[name: '*/$GIT_BRANCH']], userRemoteConfigs: [[url: 'GIT_URL']]]

                sh 'git fetch'
                sh 'git branch'
                sh 'git checkout master'
                sh 'git checkout $BRANCH_NAME'
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
