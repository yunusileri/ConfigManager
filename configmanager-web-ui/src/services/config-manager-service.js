import apiCaller from "@/helper/api-caller";

const ConfigManagerService = {
    GetList: async (appName) => {
        const url = `/config-value/${appName}`
        const response = await apiCaller.get(url)
        return response?.data
    },
    Add: async (appName, req) => {
        const url = `/config-value/${appName}`
        const response = await apiCaller.post(url, req)
        return response?.data
    }
}

export default ConfigManagerService;