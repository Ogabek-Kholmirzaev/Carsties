This is a [Next.js](https://nextjs.org) project bootstrapped with [`create-next-app`](https://nextjs.org/docs/app/api-reference/cli/create-next-app).

## Getting Started

First, run the development server:

```bash
npm run dev
# or
yarn dev
# or
pnpm dev
# or
bun dev
```

Open [http://localhost:3000](http://localhost:3000) with your browser to see the result.

You can start editing the page by modifying `app/page.tsx`. The page auto-updates as you edit the file.

This project uses [`next/font`](https://nextjs.org/docs/app/building-your-application/optimizing/fonts) to automatically optimize and load [Geist](https://vercel.com/font), a new font family for Vercel.

## Learn More

To learn more about Next.js, take a look at the following resources:

- [Next.js Documentation](https://nextjs.org/docs) - learn about Next.js features and API.
- [Learn Next.js](https://nextjs.org/learn) - an interactive Next.js tutorial.

You can check out [the Next.js GitHub repository](https://github.com/vercel/next.js) - your feedback and contributions are welcome!

## Deploy on Vercel

The easiest way to deploy your Next.js app is to use the [Vercel Platform](https://vercel.com/new?utm_medium=default-template&filter=next.js&utm_source=create-next-app&utm_campaign=create-next-app-readme) from the creators of Next.js.

Check out our [Next.js deployment documentation](https://nextjs.org/docs/app/building-your-application/deploying) for more details.

## Docker build

You can build the Docker image in two ways. The Dockerfile supports an `APP_DIR` build-arg so it works whether you set the build context to the repository root or to the `frontend/web-app` folder.

1) Build from inside the web-app folder (simple):

```powershell
cd frontend\web-app
docker build -t ogabekkholmirzaev/web-app:latest .
```

2) Build from the repository root and pass the app directory as a build-arg (recommended for CI):

```powershell
# From repository root - pass build context as the web-app folder
docker build -t ogabekkholmirzaev/web-app:latest -f frontend\web-app\Dockerfile --build-arg APP_DIR=frontend/web-app frontend/web-app

# Or using buildx and push
docker buildx build --push -t ogabekkholmirzaev/web-app:latest -f frontend\web-app\Dockerfile --build-arg APP_DIR=frontend/web-app frontend/web-app
```

Notes:
- The Dockerfile will default to `APP_DIR=.` so building from inside `frontend/web-app` continues to work.
- Use `.dockerignore` to avoid sending large directories (node_modules, .next, .git) into the build context.
