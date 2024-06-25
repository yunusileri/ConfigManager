import configManagerService from "@/services/config-manager-service";
import {useEffect, useState} from "react";
import {DataGrid} from "@mui/x-data-grid";
import {Box, Button} from "@mui/material";
import AddOrUpdateValue from "@/components/add-or-update-value";

const IndexPage = () => {
    const [rows, setRows] = useState([])
    const [editRow, setEditRow] = useState(null)
    const [openNewKey, setOpenNewKey] = useState(false)

    const init = async () => {
        const response = await configManagerService.GetList('Service1')
        console.log(response)
        setRows(response)
    }
    useEffect(() => {
        init()
    }, []);

    const columns = [
        {field: 'appName', headerName: 'Servis Adı', flex: 1},
        {field: 'key', headerName: 'Key', flex: 1},
        {field: 'value', headerName: 'Value', flex: 1},
        {
            field: '#',
            headerName: 'Düzenle',
            flex: 1,
            renderCell: params => {
                const {row} = params

                return (
                    <>
                        <Button variant={'contained'} onClick={e => setEditRow(row)}>
                            Düzenle
                        </Button>
                    </>
                )
            }
        }
    ]

    return <>
        <Box mb={3}>
            <Button onClick={e => setOpenNewKey(true)} variant='contained'>Yeni Key Oluştur</Button>
        </Box>

        {
           ( editRow != null || openNewKey) &&
            <AddOrUpdateValue row={editRow} open={editRow != null || openNewKey} closeModal={() => {
                setEditRow(null)
                setOpenNewKey(false)
                init()
            }}/>
        }


        <DataGrid columns={columns} rows={rows}/>
    </>
}

export default IndexPage