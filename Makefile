.PHONY: build test test-e2e test-a11y tf-f tf-v

# .NET Targets
build:
	dotnet clean
	dotnet restore --locked-mode
	dotnet format
	dotnet build --no-restore

# Test Targets
test:
	dotnet test tests/AccessingChildcareEntitlementChecker.UnitTests --no-build --results-directory ./test-results --logger "trx" /p:CollectCoverage=true /m:1
	dotnet test tests/AccessingChildcareEntitlementChecker.IntegrationTests --no-build --results-directory ./test-results --logger "trx" /p:CollectCoverage=true /m:1

test-e2e:
	dotnet test tests/AccessingChildcareEntitlementChecker.E2eTests --no-build --logger:"console;verbosity=normal"

test-a11y:
	dotnet test tests/AccessingChildcareEntitlementChecker.A11yTests --no-build --logger:"console;verbosity=normal"

# Terraform Targets
tf-f:
	terraform fmt -recursive infra/

tf-v:
	terraform -chdir=infra/terraform validate
