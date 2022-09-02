#!/usr/bin/env bash

set -euo pipefail

# enable debug
# set -x

echo "configuring dynamodb"
echo "==================="
echo "Users Table being created..."

# https://gugsrs.com/localstack-sqs-sns/
LOCALSTACK_HOST=localhost
CLI="aws --endpoint-url=http://${LOCALSTACK_HOST}:4566"

## change this code to create the table related to your service
TABLE_NAME="Users"
create_table() {
  ${CLI} dynamodb create-table \
    --table-name ${TABLE_NAME} \
    --attribute-definitions AttributeName=UserName,AttributeType=S AttributeName=Id,AttributeType=N \
    --key-schema AttributeName=UserName,KeyType=HASH AttributeName=Id,KeyType=RANGE  \
    --provisioned-throughput ReadCapacityUnits=5,WriteCapacityUnits=5
}

echo "creating dynamodb"
echo "$(create_table)"
