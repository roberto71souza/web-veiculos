# Web Veículos
Sistema web que consome dados de uma API através de requisições Ajax.
Neste projeto é aplicado alguns conceitos DDD e arquitetura limpa como arquitetura em camadas, domínio, repositório, serviços, injeção de dependência e inversão de dependência.

# Docker
Docker é uma ferramenta que facilita o desenvolvimento de sistemas para a equipe de desenvolvedores, com o Docker é possível configurar e criar o ambiente de desenvolvimento que pode ser compartilhado junto do projeto a equipe dos desenvolvedores sem a necessidade que a maquina local tenha instalado as ferramentas ou tecnologias já configuradas pelo Docker, isto porque a ferramenta Docker administra os sistemas em contêineres, que é isolado do sistema operacional da máquina local.
Uma ferramenta do Docker que é muito interessante, que é usada também neste projeto, é o Docker Compose, ele é bastante usado em projetos mais complexos que possui múltiplos sistemas, como por exemplo aplicações de api's, front end e banco de dados, com o Docker Compose é possível em um único arquivo configurar e criar o ambiente destas aplicações e rodar estas aplicações somente com um único comando.

# Principais Tecnologias usadas:
**Back-end:**
- .Net 5
- Dapper
- Autommaper
- Testes com xUnit
- Banco de dados MySQL

**Front-end:**
- HTML
- CSS
- Javascript/JQuery

**Versionamento:**
- Git e Github

**Administração de ambientes:**
- Docker

# Executar o sistema usando o Docker

- Clonar este projeto, ou fazer o download zip.

- Na pasta raiz do projeto, aonde possui o arquivo docker-compose.yml, executar o programa prompt de comando.

- Executar o seguinte comando: docker-compose up -d.

- Após o container ser construído e executado, é possível acessar as trés aplicações nos seguintes endereços:

    * 1 . API: localhost:5000/veiculos.
    * 2 . Aplicação front end: localhost:3000.
    * 3 . Banco de dados:
        + **servidor:** 127.0.0.1 ou localhost.
        + **usuário:** root.
        + **senha:** usuarioX96.
        + **porta:** 3306.

- Para parar a execução do container basta executar o comando docker-compose stop.

- Para executar o container outras vezes após parar o container, basta executar o comando docker-compose start ou executar dentro do aplicativo docker.