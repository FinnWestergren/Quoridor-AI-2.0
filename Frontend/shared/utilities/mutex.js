
const locks = {}

export const useMutex = async (callback, id) => {
    if (locks[id]) {
        return
    }
    try {
        locks[id] = true;
        return await callback()
    }
    catch {
        return
    }
    finally {
        locks[id] = false
    }
}