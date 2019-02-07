COMPOSE = docker-compose

UID ?= $(shell id -u)
RUN_WEB = $(COMPOSE) run -u $(UID) --rm web
RUN_WEB_SERVICE = $(COMPOSE) run -u $(UID) --rm --service-ports web


.PHONY: \
		test test-homes-england test-web-api test-acceptance \
		setup serve build stop shell \
		docker-build docker-down docker-stop \


test: test-all

test-acceptance: build
	$(RUN_WEB) dotnet watch --project AcceptanceTest test

test-homes-england-gateway: build
	$(RUN_WEB) dotnet watch --project HomesEngland.Gateway.Test test
	
test-homes-england: build
	$(RUN_WEB) dotnet watch --project HomesEnglandTest test

test-infrastructure: build
	$(RUN_WEB) dotnet watch --project InfrastructureTest test

test-web-api: build
	$(RUN_WEB) dotnet watch --project WebApiTest test

test-all: build
	$(RUN_WEB) dotnet test


setup: build
	# This is implicit: $(RUN_WEB) dotnet restore

serve: setup
	$(RUN_WEB_SERVICE) dotnet watch --project WebApi run 

build: docker-build

stop: docker-stop

shell:
	$(RUN_WEB) bash

docker-build:
	$(COMPOSE) build

docker-down:
	$(COMPOSE) down

docker-stop:
	$(COMPOSE) stop

seeds:
	$(RUN_WEB_SERVICE) dotnet run --project HomesEngland.Gateway.DataGenerator --records 100
