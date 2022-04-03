
const locks = {}

export const useMutex = async (callback, id) => {
    console.log(locks)
    while (locks[id]) {
        await new Promise(r => setTimeout(r, 100));
    }
    try {
        locks[id] = true;
        return await callback()
    }
    catch {
        return false
    }
    finally {
        locks[id] = false
    }
}