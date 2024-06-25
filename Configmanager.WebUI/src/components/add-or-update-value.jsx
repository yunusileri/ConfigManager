import {Autocomplete, Button, Dialog, DialogContent, DialogTitle, Grid, TextField, Typography} from "@mui/material";
import * as React from "react";
import {useEffect, useState} from "react";
import {ConfigValueTypeObjects} from "@/enum/config-value-type";
import ConfigManagerService from "@/services/config-manager-service";


const AddOrUpdateValue = ({row, open, closeModal}) => {
    const [form, setForm] = useState({
        key: '',
        value: '',
        type: ConfigValueTypeObjects[0],
        appName: ''
    })

    useEffect(() => {
        if (row == null) {
            setForm({
                key: '',
                value: '',
                type: ConfigValueTypeObjects[0],
                appName: ''
            })
        } else {
            setForm({...row, type: row.type})
        }
    }, [row])

    const save = async () => {

        const response = await ConfigManagerService.Add(form.appName, form)
        console.log("=>(add-or-update-value.jsx:32) response", response);
        closeModal()
        alert('Kaydedildi.')
    }

    return <>
        <Dialog onClose={closeModal} aria-labelledby='customized-dialog-title' open={open} maxWidth={'lg'} fullWidth>
            <DialogTitle id='customized-dialog-title' sx={{p: 4}}>
                <Typography variant='h6' component='span'>
                    Değer Ekle/Güncelle
                </Typography>

            </DialogTitle>
            <DialogContent dividers sx={{p: 4}}>
                <Grid container spacing={3}>
                    <Grid item xs={12}>
                        <TextField fullWidth label={'ServisName'} value={form.appName}
                                   onChange={event => setForm({...form, appName: event.target.value})}/>
                    </Grid>

                    <Grid item xs={12}>
                        <TextField fullWidth label={'Key'} value={form.key}
                                   onChange={event => setForm({...form, key: event.target.value})}/>
                    </Grid>
                    <Grid item xs={12}>
                        <TextField fullWidth label={'Value'} value={form.value}
                                   onChange={event => setForm({...form, value: event.target.value})}/>
                    </Grid>
                    <Grid item xs={12}>
                        <Autocomplete
                            value={form.type}
                            renderInput={params => <TextField {...params} label='Type'/>}
                            options={ConfigValueTypeObjects}
                            onChange={(event, value) => {
                                console.log("=>(add-or-update-value.jsx:47) value", value);
                                setForm({...form, type: value})
                            }}
                            getOptionLabel={opt => opt}
                        />
                    </Grid>

                    <Grid item xs={12} display={'flex'} justifyContent={'end'}>
                        <Button variant='contained' onClick={save}>Kaydet</Button>
                    </Grid>
                </Grid>

            </DialogContent>
        </Dialog>


    </>
}


export default AddOrUpdateValue


 