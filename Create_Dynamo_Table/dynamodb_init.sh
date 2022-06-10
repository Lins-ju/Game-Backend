#!/usr/bin/env bash

set -euo pipefail

# enable debug
# set -x

echo "configuring dynamodb"
echo "==================="

# https://gugsrs.com/localstack-sqs-sns/
LOCALSTACK_HOST=localhost
CLI="aws --endpoint-url=http://${LOCALSTACK_HOST}:8000"

## change this code to create the table related to your service
TABLE_NAME="LeaderboardData"
create_table() {
  ${CLI} dynamodb create-table \
    --table-name ${TABLE_NAME} \
    --attribute-definitions AttributeName=TrackId,AttributeType=S AttributeName=UserId,AttributeType=S \
    --key-schema AttributeName=TrackId,KeyType=HASH AttributeName=UserId,KeyType=RANGE  \
    --provisioned-throughput ReadCapacityUnits=5,WriteCapacityUnits=5
}

echo "creating dynamodb"
echo "$(create_table)"
