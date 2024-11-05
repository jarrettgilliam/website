# Website

This repository contains the source for my [website](https://www.jarrettgilliam.com).

## Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [bun](https://bun.sh/)
- [Docker](https://www.docker.com/get-started)

### Building and Running Locally

Clone the repo first:

```sh
git clone https://github.com/jarrettgilliam/website
cd website
```

#### Back-end

1. cd into the api folder:

    ```sh
    cd src/api/Website
    ```

2. Restore the dependencies:

    ```sh
    dotnet restore
    ```

3. Set the required app secrets:
    ```sh
    dotnet user-secrets init
    dotnet user-secrets set "AppSecrets:ReCaptchaSecret" "SECRET"
    dotnet user-secrets set "AppSecrets:EmailLink" "mailto:youremail@example.com"
    dotnet user-secrets set "AppSecrets:ResumeLink" "https://www.example.com/resume.pdf"
    ```

4. Build the project:
    ```sh
    dotnet build
    ```

5. Run the project:
    ```sh
    dotnet run
    ```

#### Front-end

1. cd into the web folder:

    ```sh
    cd src/web
    ```

2. Restore the dependencies:

    ```sh
    bun install
    ```

3. Run the project:

    ```sh
    bun run dev
    ```

### Building and Running with Docker

1. Set the required app secrets as environment variables:

    ```sh
    export AppSecrets__ReCaptchaSecret='SECRET'
    export AppSecrets__EmailLink='mailto:youremail@example.com'
    export AppSecrets__ResumeLink='https://www.example.com/resume.pdf'
    ```

2. Run the included `docker-compose.yml` file:

    ```sh
    docker compose up -d
    ```

3. Open your browser and navigate to http://localhost:8080.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
