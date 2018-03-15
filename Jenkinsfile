pipeline {
    agent any
    triggers {
        pollSCM 'H/30 * * * *'
    }
    stages {
        
        stage('Graduate') {
            steps {
                sh 'git fetch master'
                sh 'git checkout $BRANCH_NAME'
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
