import { IconButton, Box, Input, useSafeLayoutEffect } from "@chakra-ui/react";
import React, { useEffect, useRef, useState } from "react";
import { MdUploadFile } from "react-icons/md";
export default function Upload({fileInfo,Uploading,setFile}) {
  const fileInputRef = useRef();
  const [buttonDisabled,setButtonDisabled]=useState(false)
  const handleButtonClick = () => {
    fileInputRef.current.click();
  };
  useEffect(()=>{
    if (fileInfo!=null){
      setButtonDisabled(true)
    }
  },[fileInfo])
  const checkIfFileExists=()=>{
    if (fileInfo!=null){
      return true
    } else{
      return false
    }
  } 
  const handleFileChange =async (event) => {
    Uploading(true)
    // Handle the file here, e.g., upload it to a server or read its contents
    const file=event.target.files[0]
    const formdata=new FormData()
    formdata.append('file',file)
    try {
     const res=await fetch('https://localhost:7126/api/PDF/UploadPDF',{
      method:'POST',
      body: formdata
     }) 

      if (res.ok) {
        alert('File uploaded successfully');
        setFile(file)
      } else {
        alert('File upload failed');
      }
      Uploading(false)
    } catch (error) {
     console.error(error) 
     Uploading(false)
    }
    
  };
  return (
    <Box>
      <input
        type="file"
        ref={fileInputRef}
        accept=".pdf"
        onChange={handleFileChange}
        disabled={buttonDisabled}
        style={{ display: "none" }}
      />
      <IconButton disabled={buttonDisabled} onClick={handleButtonClick} icon={<MdUploadFile size="15" />}></IconButton>
    </Box>
  );
}
