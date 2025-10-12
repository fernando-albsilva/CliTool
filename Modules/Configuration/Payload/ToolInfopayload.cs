using CliTool.Modules.CommandHelper;

namespace CliTool.Modules.Configuration.Payload
{
    public static class ToolInfoPayload
    {
        public static List<ToolArg> Tools => new List<ToolArg>
        {
            new ToolArg
            {
                Tool = "dotnet",
                Commands = new List<CommandInfo>
                {
                    new CommandInfo
                    {
                        Name = "dotnet new <template> -n <nome>",
                        Description = "Cria um novo projeto, solução ou arquivo com base em um template (ex: console, webapi)"
                    },
                    new CommandInfo
                    {
                        Name = "dotnet new list",
                        Description = "Lista todos os templates disponíveis para criação de projetos"
                    },
                    new CommandInfo
                    {
                        Name = "dotnet build",
                        Description = "Compila o projeto e gera os binários na pasta bin/"
                    },
                    new CommandInfo
                    {
                        Name = "dotnet run",
                        Description = "Compila e executa o projeto atual"
                    },
                    new CommandInfo
                    {
                        Name = "dotnet run --project <caminho>",
                        Description = "Executa um projeto específico em vez do padrão"
                    },
                    new CommandInfo
                    {
                        Name = "dotnet restore",
                        Description = "Restaura as dependências e pacotes NuGet do projeto"
                    },
                    new CommandInfo
                    {
                        Name = "dotnet clean",
                        Description = "Remove os arquivos compilados (bin e obj) do projeto"
                    },
                    new CommandInfo
                    {
                        Name = "dotnet publish -c Release -o <pasta>",
                        Description = "Publica o aplicativo compilado em modo Release no diretório especificado"
                    },
                    new CommandInfo
                    {
                        Name = "dotnet test",
                        Description = "Executa todos os testes automatizados do projeto"
                    },
                    new CommandInfo
                    {
                        Name = "dotnet test --filter <expressao>",
                        Description = "Executa apenas os testes que correspondem ao filtro especificado"
                    },
                    new CommandInfo
                    {
                        Name = "dotnet add package <nome_pacote>",
                        Description = "Adiciona um pacote NuGet ao projeto atual"
                    },
                    new CommandInfo
                    {
                        Name = "dotnet remove package <nome_pacote>",
                        Description = "Remove um pacote NuGet do projeto"
                    },
                    new CommandInfo
                    {
                        Name = "dotnet list package",
                        Description = "Lista todos os pacotes NuGet instalados no projeto"
                    },
                    new CommandInfo
                    {
                        Name = "dotnet tool list -g",
                        Description = "Lista todas as ferramentas .NET instaladas globalmente"
                    },
                    new CommandInfo
                    {
                        Name = "dotnet tool install -g <ferramenta>",
                        Description = "Instala uma ferramenta .NET globalmente (ex: dotnet-ef)"
                    },
                    new CommandInfo
                    {
                        Name = "dotnet tool update -g <ferramenta>",
                        Description = "Atualiza uma ferramenta .NET instalada globalmente"
                    },
                    new CommandInfo
                    {
                        Name = "dotnet ef migrations add <nome_migracao>",
                        Description = "Adiciona uma nova migração usando o Entity Framework Core"
                    },
                    new CommandInfo
                    {
                        Name = "dotnet ef database update",
                        Description = "Aplica as migrações pendentes ao banco de dados"
                    },
                    new CommandInfo
                    {
                        Name = "dotnet --info",
                        Description = "Exibe informações detalhadas sobre a instalação atual do .NET"
                    },
                    new CommandInfo
                    {
                        Name = "dotnet --list-sdks",
                        Description = "Lista todos os SDKs do .NET instalados"
                    },
                    new CommandInfo
                    {
                        Name = "dotnet --list-runtimes",
                        Description = "Lista todos os runtimes do .NET instalados"
                    }
                }
            },
            new ToolArg
            {
                Tool = "git",
                Commands = new List<CommandInfo>
                {
                    new CommandInfo
                    {
                        Name = "git status",
                        Description = "Mostra o estado do repositório, arquivos modificados e staged"
                    },
                    new CommandInfo
                    {
                        Name = "git add <file>",
                        Description = "Adiciona arquivos ao staging para commit"
                    },
                    new CommandInfo
                    {
                        Name = "git commit -m \"mensagem\"",
                        Description = "Cria um commit com uma mensagem descritiva"
                    },
                    new CommandInfo
                    {
                        Name = "git push",
                        Description = "Envia commits locais para o repositório remoto"
                    },
                    new CommandInfo
                    {
                        Name = "git pull",
                        Description = "Atualiza o repositório local com mudanças do remoto"
                    },
                    new CommandInfo
                    {
                        Name = "git clone <url>",
                        Description = "Clona um repositório remoto para o computador local"
                    },
                    new CommandInfo
                    {
                        Name = "git branch",
                        Description = "Lista branches existentes ou cria uma nova branch"
                    },
                    new CommandInfo
                    {
                        Name = "git checkout <branch>",
                        Description = "Troca para a branch especificada"
                    },
                    new CommandInfo
                    {
                        Name = "git merge <branch>",
                        Description = "Faz merge da branch especificada na branch atual"
                    },
                    new CommandInfo
                    {
                        Name = "git log",
                        Description = "Exibe o histórico de commits do repositório"
                    }
                }
            },
            new ToolArg
            {
                Tool = "liquibase",
                Commands = new List<CommandInfo>
                {
                    new CommandInfo
                    {
                        Name = "liquibase update",
                        Description = "Aplica todas as mudanças pendentes (changeSets) no banco de dados"
                    },
                    new CommandInfo
                    {
                        Name = "liquibase rollback <tag|count|date>",
                        Description = "Desfaz mudanças aplicadas, voltando para um estado anterior definido por tag, número de changeSets ou data"
                    },
                    new CommandInfo
                    {
                        Name = "liquibase rollbackCount <n>",
                        Description = "Desfaz os últimos N changeSets aplicados"
                    },
                    new CommandInfo
                    {
                        Name = "liquibase rollbackToDate <data>",
                        Description = "Reverte o banco de dados para o estado de uma data específica (ex: '2025-10-04')"
                    },
                    new CommandInfo
                    {
                        Name = "liquibase rollbackOneChangeSet --id=<id> --author=<autor>",
                        Description = "Reverte apenas um changeSet específico baseado em seu ID e autor"
                    },
                    new CommandInfo
                    {
                        Name = "liquibase updateSQL",
                        Description = "Gera o SQL que seria executado pelo comando 'update' sem aplicá-lo"
                    },
                    new CommandInfo
                    {
                        Name = "liquibase rollbackSQL <tag|count|date>",
                        Description = "Gera o SQL necessário para realizar o rollback sem executar no banco"
                    },
                    new CommandInfo
                    {
                        Name = "liquibase status",
                        Description = "Mostra quais changeSets ainda não foram aplicados no banco de dados"
                    },
                    new CommandInfo
                    {
                        Name = "liquibase validate",
                        Description = "Valida o changelog XML/YAML/JSON para garantir que não há erros de sintaxe ou duplicação"
                    },
                    new CommandInfo
                    {
                        Name = "liquibase clearCheckSums",
                        Description = "Remove os checksums armazenados, forçando o recalculo na próxima execução"
                    },
                    new CommandInfo
                    {
                        Name = "liquibase generateChangeLog",
                        Description = "Gera um changelog inicial com base na estrutura atual do banco de dados"
                    },
                    new CommandInfo
                    {
                        Name = "liquibase diff",
                        Description = "Compara dois bancos de dados ou um banco com um changelog e mostra as diferenças"
                    },
                    new CommandInfo
                    {
                        Name = "liquibase diffChangeLog",
                        Description = "Gera um changelog contendo as diferenças entre dois bancos"
                    },
                    new CommandInfo
                    {
                        Name = "liquibase tag <nome>",
                        Description = "Cria uma tag no banco de dados para permitir rollbacks futuros até esse ponto"
                    },
                    new CommandInfo
                    {
                        Name = "liquibase history",
                        Description = "Lista todas as mudanças aplicadas (changeSets executados) no banco"
                    },
                    new CommandInfo
                    {
                        Name = "liquibase changelogSync",
                        Description = "Marca todos os changeSets do changelog como executados, sem realmente aplicá-los"
                    },
                    new CommandInfo
                    {
                        Name = "liquibase changelogSyncSQL",
                        Description = "Gera o SQL equivalente ao 'changelogSync' sem executá-lo"
                    },
                    new CommandInfo
                    {
                        Name = "liquibase updateTestingRollback",
                        Description = "Executa o update e testa o rollback para garantir que as mudanças são reversíveis"
                    },
                    new CommandInfo
                    {
                        Name = "liquibase snapshot",
                        Description = "Cria um snapshot do estado atual do banco (em JSON ou YAML)"
                    },
                    new CommandInfo
                    {
                        Name = "liquibase help",
                        Description = "Exibe ajuda detalhada sobre os comandos disponíveis"
                    }
                }
            },
            new ToolArg
            {
                Tool = "angular",
                Commands = new List<CommandInfo>
                {
                    new CommandInfo
                    {
                        Name = "ng new <nome-projeto>",
                        Description = "Cria um novo projeto Angular com a estrutura inicial e configurações padrão"
                    },
                    new CommandInfo
                    {
                        Name = "ng serve",
                        Description = "Inicia o servidor de desenvolvimento e recompila o projeto automaticamente ao salvar alterações"
                    },
                    new CommandInfo
                    {
                        Name = "ng build",
                        Description = "Compila o projeto para produção, gerando os arquivos otimizados na pasta dist/"
                    },
                    new CommandInfo
                    {
                        Name = "ng build --configuration production",
                        Description = "Compila o projeto usando as configurações do ambiente de produção"
                    },
                    new CommandInfo
                    {
                        Name = "ng test",
                        Description = "Executa os testes unitários do projeto utilizando o Karma"
                    },
                    new CommandInfo
                    {
                        Name = "ng e2e",
                        Description = "Executa os testes de ponta a ponta (end-to-end)"
                    },
                    new CommandInfo
                    {
                        Name = "ng generate component <nome>",
                        Description = "Gera um novo componente Angular com os arquivos HTML, CSS, TS e spec"
                    },
                    new CommandInfo
                    {
                        Name = "ng generate service <nome>",
                        Description = "Cria um novo serviço Angular"
                    },
                    new CommandInfo
                    {
                        Name = "ng generate module <nome>",
                        Description = "Gera um novo módulo Angular (opcionalmente com roteamento)"
                    },
                    new CommandInfo
                    {
                        Name = "ng generate directive <nome>",
                        Description = "Cria uma nova diretiva customizada"
                    },
                    new CommandInfo
                    {
                        Name = "ng generate pipe <nome>",
                        Description = "Cria um novo pipe customizado para transformações de dados"
                    },
                    new CommandInfo
                    {
                        Name = "ng generate guard <nome>",
                        Description = "Cria um guard de rota para controle de acesso"
                    },
                    new CommandInfo
                    {
                        Name = "ng generate interceptor <nome>",
                        Description = "Cria um interceptor HTTP para manipular requisições e respostas"
                    },
                    new CommandInfo
                    {
                        Name = "ng lint",
                        Description = "Executa o lint para verificar problemas de estilo e padrões de código"
                    },
                    new CommandInfo
                    {
                        Name = "ng add <pacote>",
                        Description = "Adiciona uma biblioteca ao projeto (ex: Angular Material, PWA, Tailwind)"
                    },
                    new CommandInfo
                    {
                        Name = "ng update",
                        Description = "Atualiza dependências do Angular e outras bibliotecas associadas"
                    },
                    new CommandInfo
                    {
                        Name = "ng deploy",
                        Description = "Faz o deploy do projeto (quando configurado via @angular/fire, GitHub Pages, etc.)"
                    },
                    new CommandInfo
                    {
                        Name = "ng doc <termo>",
                        Description = "Abre a documentação oficial do Angular referente ao termo informado"
                    },
                    new CommandInfo
                    {
                        Name = "ng version",
                        Description = "Exibe a versão do Angular CLI, Angular core e dependências do projeto"
                    },
                    new CommandInfo
                    {
                        Name = "ng config <chave> <valor>",
                        Description = "Define uma configuração no arquivo angular.json ou exibe uma existente"
                    },
                    new CommandInfo
                    {
                        Name = "ng analytics",
                        Description = "Gerencia o uso de telemetria (ativar, desativar ou verificar status)"
                    },
                    new CommandInfo
                    {
                        Name = "ng extract-i18n",
                        Description = "Extrai strings traduzíveis para arquivos de internacionalização (i18n)"
                    },
                    new CommandInfo
                    {
                        Name = "ng cache clean",
                        Description = "Limpa o cache interno do Angular CLI"
                    },
                    new CommandInfo
                    {
                        Name = "ng help",
                        Description = "Exibe ajuda detalhada sobre os comandos disponíveis no Angular CLI"
                    }
                }
            },
            new ToolArg
            {
                Tool = "docker",
                Commands = new List<CommandInfo>
                {
                    new CommandInfo
                    {
                        Name = "docker ps",
                        Description = "Lista os contêineres em execução no momento"
                    },
                    new CommandInfo
                    {
                        Name = "docker ps -a",
                        Description = "Lista todos os contêineres, incluindo os que estão parados"
                    },
                    new CommandInfo
                    {
                        Name = "docker images",
                        Description = "Lista todas as imagens disponíveis localmente"
                    },
                    new CommandInfo
                    {
                        Name = "docker pull <imagem>",
                        Description = "Baixa uma imagem do Docker Hub para o ambiente local"
                    },
                    new CommandInfo
                    {
                        Name = "docker run <imagem>",
                        Description = "Cria e inicia um novo contêiner a partir de uma imagem"
                    },
                    new CommandInfo
                    {
                        Name = "docker run -d -p <porta_local>:<porta_container> <imagem>",
                        Description = "Inicia um contêiner em segundo plano (modo detached) com mapeamento de porta"
                    },
                    new CommandInfo
                    {
                        Name = "docker exec -it <container> <comando>",
                        Description = "Executa um comando dentro de um contêiner em execução"
                    },
                    new CommandInfo
                    {
                        Name = "docker stop <container>",
                        Description = "Para a execução de um contêiner"
                    },
                    new CommandInfo
                    {
                        Name = "docker start <container>",
                        Description = "Inicia um contêiner parado"
                    },
                    new CommandInfo
                    {
                        Name = "docker rm <container>",
                        Description = "Remove um contêiner parado"
                    },
                    new CommandInfo
                    {
                        Name = "docker rmi <imagem>",
                        Description = "Remove uma imagem local"
                    },
                    new CommandInfo
                    {
                        Name = "docker build -t <nome_imagem> .",
                        Description = "Cria uma nova imagem Docker a partir de um Dockerfile no diretório atual"
                    },
                    new CommandInfo
                    {
                        Name = "docker-compose up -d",
                        Description = "Inicia os serviços definidos no arquivo docker-compose.yml em modo detached"
                    },
                    new CommandInfo
                    {
                        Name = "docker-compose down",
                        Description = "Para e remove os contêineres definidos no docker-compose.yml"
                    },
                    new CommandInfo
                    {
                        Name = "docker logs <container>",
                        Description = "Exibe os logs de um contêiner em execução ou já parado"
                    },
                    new CommandInfo
                    {
                        Name = "docker system prune -a",
                        Description = "Remove todos os recursos não utilizados (contêineres, imagens e volumes órfãos)"
                    }
                }
            },
            new ToolArg
            {
                Tool = "network",
                Commands = new List<CommandInfo>
                {
                    new CommandInfo
                    {
                        Name = "ping <host>",
                        Description = "Verifica a conectividade com um host (ex: ping google.com)"
                    },
                    new CommandInfo
                    {
                        Name = "tracert <host>",
                        Description = "Exibe o caminho percorrido até o destino (Windows). Em Linux, use 'traceroute <host>'"
                    },
                    new CommandInfo
                    {
                        Name = "ipconfig",
                        Description = "Mostra as configurações de IP da máquina (Windows). Em Linux, use 'ifconfig' ou 'ip addr'"
                    },
                    new CommandInfo
                    {
                        Name = "ipconfig /all",
                        Description = "Exibe informações detalhadas sobre adaptadores de rede (Windows)"
                    },
                    new CommandInfo
                    {
                        Name = "ipconfig /flushdns",
                        Description = "Limpa o cache de DNS local (Windows)"
                    },
                    new CommandInfo
                    {
                        Name = "nslookup <domínio>",
                        Description = "Consulta o servidor DNS para resolver nomes de domínio"
                    },
                    new CommandInfo
                    {
                        Name = "netstat -an",
                        Description = "Mostra todas as conexões e portas TCP/UDP abertas"
                    },
                    new CommandInfo
                    {
                        Name = "telnet <host> <porta>",
                        Description = "Testa a conectividade de uma porta específica em um host"
                    },
                    new CommandInfo
                    {
                        Name = "curl <url>",
                        Description = "Faz requisições HTTP/HTTPS para testar endpoints"
                    },
                    new CommandInfo
                    {
                        Name = "wget <url>",
                        Description = "Baixa arquivos da internet via linha de comando (Linux/macOS)"
                    },
                    new CommandInfo
                    {
                        Name = "arp -a",
                        Description = "Lista os dispositivos conectados à rede local e seus endereços MAC"
                    },
                    new CommandInfo
                    {
                        Name = "route print",
                        Description = "Exibe a tabela de rotas da máquina (Windows). Em Linux: 'route -n'"
                    },
                    new CommandInfo
                    {
                        Name = "netsh wlan show profile",
                        Description = "Lista redes Wi-Fi salvas no Windows"
                    },
                    new CommandInfo
                    {
                        Name = "netsh wlan show profile <SSID> key=clear",
                        Description = "Mostra os detalhes (incluindo senha) de uma rede Wi-Fi salva"
                    }
                }
            }
        };
    }
}