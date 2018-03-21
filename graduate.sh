#!/bin/bash
BRANCH=$(git branch | sed -n -e 's/^\* \(.*\)/\1/p')

if [ "$BRANCH" = "dev" ]
then
  echo "Graduating dev branch to master branch"

  # Fetch other branches
  git fetch origin && \

  # Pull the master branch into the dev branch
  git pull origin master && \

  # Checkout the master branch
  git checkout master && \

  # Ensure it's up to date
  git pull origin master && \

  # Merge the dev branch
  git merge -q dev && \

  # Push the updates
  git push origin master && \

  # Go back to the dev branch
  git checkout dev && \

  echo "The 'dev' branch has been graduated to the 'master' branch"  || \

  (echo "Error" && exit 1)
else
  echo "You must be in the 'dev' branch to graduate to the 'master' branch, but currently in the '$BRANCH' branch. Skipping."
fi
date
