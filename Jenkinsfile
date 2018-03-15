pipeline {
    agent any
    triggers {
        pollSCM 'H/30 * * * *'
    }
    stages {
        
        stage('Checkout') {
            steps {
                
                checkout([$class: 'GitSCM', branches: [[name: '*/$BRANCH_NAME']]])
                checkout([$class: 'GitSCM', branches: [[name: '*/master']]])

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
