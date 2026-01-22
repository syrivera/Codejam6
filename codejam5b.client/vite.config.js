import { fileURLToPath, URL } from "node:url";

import { defineConfig } from "vite";
import plugin from "@vitejs/plugin-react";
import fs from "fs";
import path from "path";
import child_process from "child_process";
import { env } from "process";

const isCI = env.CI === "true" || env.GITHUB_ACTIONS === "true";

// Only configure HTTPS certs for local dev server, never for CI builds.
function getHttpsConfig() {
    if (isCI) return false;

    const baseFolder =
        env.APPDATA !== undefined && env.APPDATA !== ""
            ? `${env.APPDATA}/ASP.NET/https`
            : `${env.HOME}/.aspnet/https`;

    const certificateName = "codejam5b.client";
    const certFilePath = path.join(baseFolder, `${certificateName}.pem`);
    const keyFilePath = path.join(baseFolder, `${certificateName}.key`);

    if (!fs.existsSync(baseFolder)) {
        fs.mkdirSync(baseFolder, { recursive: true });
    }

    if (!fs.existsSync(certFilePath) || !fs.existsSync(keyFilePath)) {
        const result = child_process.spawnSync(
            "dotnet",
            [
                "dev-certs",
                "https",
                "--export-path",
                certFilePath,
                "--format",
                "Pem",
                "--no-password",
            ],
            { stdio: "inherit" }
        );

        if (result.status !== 0) {
            throw new Error("Could not create certificate.");
        }
    }

    return {
        key: fs.readFileSync(keyFilePath),
        cert: fs.readFileSync(certFilePath),
    };
}

const target = env.ASPNETCORE_HTTPS_PORT
    ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}`
    : env.ASPNETCORE_URLS
        ? env.ASPNETCORE_URLS.split(";")[0]
        : "http://localhost:5205";

// https://vitejs.dev/config/
export default defineConfig(({ command }) => {
    const isDevServer = command === "serve";
    const httpsConfig = isDevServer ? getHttpsConfig() : false;

    return {
        plugins: [plugin()],
        resolve: {
            alias: {
                "@": fileURLToPath(new URL("./src", import.meta.url)),
            },
        },
        server: {
            proxy: {
                "^/weatherforecast": {
                    target,
                    secure: false,
                },
                "^/api": {
                    target,
                    secure: false,
                },
            },
            port: parseInt(env.DEV_SERVER_PORT || "63613"),
            https: httpsConfig,
        },
    };
});