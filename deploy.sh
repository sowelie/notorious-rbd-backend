sam package \
  --template-file template.yml \
  --output-template notorious-rbd.yml \
  --s3-bucket notorious-rbd-deploy

sam deploy \
--template-file notorious-rbd.yml \
--stack-name notorious-rdb \
--capabilities CAPABILITY_IAM \
--region us-east-2