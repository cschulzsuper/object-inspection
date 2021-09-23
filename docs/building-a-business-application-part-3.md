# Hey.

The Getting Started process for new developers in big software project is often an accumulation of dependencies, folder and configuration scripts. A lot of things need to be executed in the correct order. A tiny mistake in the setup might confuse the tool chain and result in a huge amount of time wasted. Getting Started should be easy.

In a recent ASP.NET Community Standup, that was all about contributing to ASP.NET core I saw GitHub Codespaces productively used for the first time. I'm not the type who wants to contribute to ASP.NET Core but I liked that codepages was actually used by the team. Safaia Abdulla showed how she uses Codespaces to run and build aspnet core in the browser.

# The Short Summary.

Magic!

# The Long Summary.

A Codespace is basically a Docker environment with Visual Studio Code on top. Docker runs in the cloud and Visual Studio Code runs in the browser. A local toolchain is not necessary.

The configuration for a Codespace is stored in a devcontainer setup, which needs to be part of the repository. While Codespaces do not work without the devcontaier, it is possible to run the devcontainer without Codespaces, on your local machine. Visual Studio Code and Docker are the only requirements.

# I want that to.

I immediately wanted a Codespace for my experimental business application as well. Sadly, Codespaces is only available for teams, enterprises and beta users. I’m not a beta user, I’m not a team and I’m certainly not an enterprise.

This leaves me with a local devcontainer that I can use once Codespaces is available to me.

As I had some experience with Docker, I know I need two containers in a composed environment. One container is my AZURE Cosmos DB, while the other is the actual devcontainer that builds my project.

There is already a lot of information regarding the setup of a devcontainer, for that reason I will not spend much time on the initial setup. I will rather focus on my pitfalls.

I started with the devcontainer for C# and MS SQL server, and replaced the MS SQL server part in the docker-compose file with AZURE Cosmos DB. I found the necessary docker-compose part for it via grep.app. A website that is the icing on the cake on what I took out of the mentioned Community Standup. It searches a high amount of GitHub Repositories amazingly fast.

The Github Repository contained the necessary settings for docker-compose.

[Cosmos](./bucket/ec521429-3387-40cf-9e87-9a0c018983f8.png)

The Azure Cosmos DB container alone is not enough, as it is running with https I need to trust its certificate. The following shell script will copy the certificate into the devcontainer.

[Cert](./bucket/d8762fbf-de08-4471-9e3a-c510a92144d5.png)

The standard devcontainer for .NET doesn’t allow .NET 6 yet, which made it necessary to go a similar route as the ASP.NET Core project. I build .NET 6 myself in the devcontainer. The ASP.NET Core project uses a lot of scripts, as they clearly have other requirements. I just use the official .NET install script.

[DotnetInstall](./bucket/e87c3463-1794-4ee9-9fa1-82e72db853e8.png)

After some final configuration in the devcontainer, I was able to compile the server project.

[RemoteEnv](./bucket/5f96dcda-96e6-4488-8955-48401d6c874c.png)

I got another certificate error as they are my kryptonite. The certificate from Azure Cosmos DB for localhost, but in the container configuration I don’t use localhost as host name I use cosmos.

As the devcontainer is only for development, I have the option to disable some of the security magic related to the certificate. I modified the cosmos connection in my EF Core configuration.

[EfCore](./bucket/456b9da7-58e9-4494-8ec5-af03eace743e.png)

I also needed to extend the script where the certificate of the Azure Cosmos DB is imported into the certificate store in the devcontainer. I use wait for it.sh for that.

[Wait](./bucket/ea926f69-3579-40c6-90d0-bbda85232966.png)

And that was it for the most part. The result can be seen in my repository. One problem that is left, and which makes me feel that we might not safe the planet after all, is that the Docker container for Azure Cosmos DB is rushing my CPU, even when my application is not running. This might be an issue of the Azure Cosmos DB container, but I’m unsure.

Overall this was another fun thing to experiment with, I’m not sure I will regularly use the devcontainer, as I’m quite comfortable with Visual Studio 2022. But maybe I need to work with devcontainer or Codespaces in the future. I'm prepared for it.

Bye.