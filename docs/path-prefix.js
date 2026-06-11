export const pathPrefix =
    process.env.ELEVENTY_ENV === "production"
        ? "/accessing-childcare-entitlement-checker/"
        : "/";