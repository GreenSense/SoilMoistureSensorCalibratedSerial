pipeline {
    agent any
    triggers {
        pollSCM 'H/10 * * * *'
    }
    stages {
        stage('Checkout') {
            steps {
                checkout scm

                sh "git config --add remote.origin.fetch +refs/heads/master:refs/remotes/origin/master"
                sh "git fetch --no-tags"
                sh 'git checkout $BRANCH_NAME'
                sh 'cp -f /usr/local/jenkins/garden-credentials.sh garden-credentials.sh'
            }
        }
        stage('Deploy') {
            steps {
                sh 'sh deploy.sh'
            }
        }
    }
    post {
        always {
            cleanWs()
        }
    }
}
