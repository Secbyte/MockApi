## Lambda deployment commands
.PHONY: build
build:
	dotnet restore ./Server
	dotnet lambda package --project-location ./Server --configuration release --framework netcoreapp2.1 --output-package ./bin/mock-api.zip

.PHONY: deploy
deploy: build
deploy: 
	serverless deploy --stage=${STAGE} --aws-profile ${PROFILE}