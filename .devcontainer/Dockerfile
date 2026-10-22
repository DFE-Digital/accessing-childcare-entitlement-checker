FROM mcr.microsoft.com/devcontainers/dotnet:2-10.0

# Install OS dependencies required by Playwright (Ubuntu/Debian)
# Should speed up initialisation
RUN apt-get update && apt-get install -y --no-install-recommends \
    libnss3 \
    libnspr4 \
    libatk1.0-0 \
    libatk-bridge2.0-0 \
    libcups2 \
    libdrm2 \
    libxkbcommon0 \
    libxcomposite1 \
    libxdamage1 \
    libxfixes3 \
    libxrandr2 \
    libgbm1 \
    libasound2t64 \
    libpangocairo-1.0-0 \
    libpango-1.0-0 \
    libcairo2 \
    libx11-6 \
    libx11-xcb1 \
    libxcb1 \
    libxext6 \
    libglib2.0-0 \
    ca-certificates \
    fonts-liberation \
    nodejs \
    npm \
    && rm -rf /var/lib/apt/lists/*

ENV PLAYWRIGHT_BROWSERS_PATH=/ms-playwright

# Install browser binaries, pinned to 1.58.0
RUN npx -y playwright@1.58.0 install chromium firefox webkit

# Terraform tooling, pinned to the versions used in CI
ARG TARGETARCH
ARG TERRAFORM_VERSION=1.9.8
ARG TERRAFORM_DOCS_VERSION=0.19.0
RUN apt-get update && apt-get install -y --no-install-recommends unzip \
    && rm -rf /var/lib/apt/lists/* \
    && curl -fsSL "https://releases.hashicorp.com/terraform/${TERRAFORM_VERSION}/terraform_${TERRAFORM_VERSION}_linux_${TARGETARCH}.zip" -o /tmp/terraform.zip \
    && unzip /tmp/terraform.zip terraform -d /usr/local/bin \
    && curl -fsSL "https://github.com/terraform-docs/terraform-docs/releases/download/v${TERRAFORM_DOCS_VERSION}/terraform-docs-v${TERRAFORM_DOCS_VERSION}-linux-${TARGETARCH}.tar.gz" \
    | tar -xz -C /usr/local/bin terraform-docs \
    && rm /tmp/terraform.zip
