# Website

This repository contains the source code for my website hosted at https://www.jarrettgilliam.com.

## Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/get-started)

### Building and Running Locally

1. Clone the repository:
    ```sh
    git clone https://github.com/jarrettgilliam/website
    cd website
    ```

2. Restore the dependencies:
    ```sh
    dotnet restore src/Website/Website.csproj
    ```
3. Set the required app secrets:
    ```sh
    dotnet user-secrets init --project src/Website/Website.csproj
    dotnet user-secrets set "AppSecrets:ReCaptchaSecret" "SECRET" --project src/Website/Website.csproj
    dotnet user-secrets set "AppSecrets:EmailLink" "mailto:youremail@example.com" --project src/Website/Website.csproj
    dotnet user-secrets set "AppSecrets:ResumeLink" "https://www.example.com/resume.pdf" --project src/Website/Website.csproj
    ```

4. Build the project:
    ```sh
    dotnet build src/Website/Website.csproj
    ```

5. Run the project:
    ```sh
    dotnet run --project src/Website/Website.csproj
    ```

### Building and Running with Docker

1. Build the Docker image:
    ```sh
    docker buildx build --platform linux/amd64,linux/arm64 -t jarrettgilliam/website "./src/Website"
    ```

2. Adjust and run this Docker compose file:
    ```yaml
    version: '3.9'
    services:
      website:
        image: jarrettgilliam/website
        container_name: website
        restart: unless-stopped
        environment:
          - AppSecrets__ReCaptchaSecret=SECRET
          - AppSecrets__EmailLink=mailto:youremail@example.com
          - AppSecrets__ResumeLink=https://www.example.com/resume.pdf
        ports:
          - "8080:8080"
    ```

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.