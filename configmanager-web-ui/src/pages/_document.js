import {Head, Html, Main, NextScript} from "next/document";

export default function Document() {
    return (
        <Html lang="tr">
            <Head/>
            <link rel="preconnect" href="https://fonts.googleapis.com"/>
            <link rel="preconnect" href="https://fonts.gstatic.com" crossOrigin/>
            <link
                rel="stylesheet"
                href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap"
            />

            <body>
            <Main/>
            <NextScript/>
            </body>
        </Html>
    );
}
