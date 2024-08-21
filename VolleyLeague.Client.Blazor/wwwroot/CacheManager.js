async function openCacheStorage() {
    return await window.caches.open("BlazorCache");
}

export async function clearCache() {
    let cache = await openCacheStorage();
    let keys = await cache.keys();
    for (let request of keys) {
        await cache.delete(request);
    }
}

export function setVersion(version) {
    localStorage.setItem("appVersion", version);
}

export function getVersion() {
    return localStorage.getItem("appVersion");
}
