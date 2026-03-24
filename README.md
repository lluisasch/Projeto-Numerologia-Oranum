# Oranum

Oranum e uma experiencia web mistica premium que revela um **Mapa Energetico do Nome** com IA, numerologia deterministica e leitura simbolica elegante. O projeto foi estruturado como monorepo com frontend React/Vite e backend ASP.NET Core Web API, persistencia em PostgreSQL e empacotamento Docker.

## Visao geral

O fluxo principal entrega:

- leitura do nome com energia geral, numero principal, arquétipo, forcas, desafios e conselho espiritual
- leitura complementar da data com signo solar, elemento, caminho de vida, missao e potenciais
- compatibilidade entre duas pessoas com score, afinidades, pontos fortes e pontos de atencao
- aviso discreto de que todo o conteudo e interpretativo para autoconhecimento e entretenimento

## Stack

### Frontend

- React 19
- TypeScript
- Vite
- Tailwind CSS
- Framer Motion
- React Router
- React Hook Form
- Zod
- Axios
- Lucide React
- componentes elegantes no estilo glass/premium

### Backend

- ASP.NET Core Web API (.NET 9)
- C# com organizacao em camadas (`Api`, `Application`, `Domain`, `Infrastructure`)
- HttpClient tipado para OpenAI
- Entity Framework Core
- PostgreSQL
- rate limiting, tratamento de erros, logs e health endpoint

## Estrutura

```text
/backend
  /Oranum.Api
  /Oranum.Application
  /Oranum.Domain
  /Oranum.Infrastructure
/frontend
```

## Requisitos locais

- .NET SDK 9.0+
- Node.js 22+
- PostgreSQL 15+
- chave da OpenAI para ativar a camada de IA

## Variaveis de ambiente

Copie `.env.example` para `.env` e ajuste os valores necessarios.

Principais variaveis:

- `ConnectionStrings__DefaultConnection`
- `OpenAI__ApiKey`
- `OpenAI__BaseUrl`
- `OpenAI__Model`
- `Cors__AllowedOrigins__0`
- `VITE_API_BASE_URL`

## Como rodar localmente

### 1. Banco de dados

Suba um PostgreSQL local ou use Docker. String padrao esperada:

```bash
Host=localhost;Port=5432;Database=oranum;Username=oranum;Password=oranum
```

### 2. Backend

```bash
cd backend/Oranum.Api
dotnet restore
dotnet run
```

API padrao: `http://localhost:8080`

Endpoints principais:

- `GET /api/health`
- `POST /api/reading/name`
- `POST /api/reading/birthdate`
- `POST /api/reading/compatibility`

### 3. Frontend

```bash
cd frontend
npm install
npm run dev
```

App padrao: `http://localhost:5173`

## Como rodar com Docker

```bash
docker compose up --build
```

Servicos esperados:

- frontend: `http://localhost:3000`
- backend: `http://localhost:8080`
- postgres: `localhost:5432`

O backend aplica `Database.Migrate()` na inicializacao, entao as migrations incluídas ja sobem automaticamente quando o banco estiver disponivel.

## Decisoes tecnicas

- A numerologia do nome e o caminho de vida sao calculados de forma deterministica no backend.
- A astrologia basica e o score de compatibilidade tambem partem de regras internas previsiveis.
- A OpenAI entra como camada de enriquecimento textual, sempre orientada por prompts estruturados e retorno em JSON.
- Existe uma camada de `knowledge providers` para permitir futuras fontes internas ou externas sem depender de scraping fragil.
- Se a IA falhar ou retornar payload inconsistente, o sistema cai para fallback seguro e ainda entrega uma leitura valida.

## Persistencia

As tabelas iniciais salvas em PostgreSQL sao:

- `name_readings`
- `birthdate_readings`
- `compatibility_readings`

Cada registro armazena o payload final em `jsonb`, identificadores principais da leitura e timestamp UTC.

## Frontend

O frontend inclui:

- landing page persuasiva com copy em PT-BR
- visual premium com gradientes, glow, glassmorphism e animacoes suaves
- formulários validados com feedback por toast
- paginas de resultado para nome, data e compatibilidade
- favicon, assets de marca, SEO basico, OG image, `robots.txt` e `sitemap.xml`

## Proximos passos sugeridos

- autenticação e perfis de usuario
- paywall e creditos por leitura
- historico de leituras por conta
- painel administrativo com analytics
- ingestao de bases simbolicas curadas por `knowledge providers`
- observabilidade externa e tracing

## Aviso do produto

Conteudo interpretativo para autoconhecimento e entretenimento.
