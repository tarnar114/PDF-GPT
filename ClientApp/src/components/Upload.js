import { IconButton, Box, Input } from "@chakra-ui/react";
import React, { useRef } from "react";
import { MdUploadFile } from "react-icons/md";
export default function Upload() {
  const fileInputRef = useRef();

  const handleButtonClick = () => {
    fileInputRef.current.click();
  };

  const handleFileChange =async (event) => {
    // Handle the file here, e.g., upload it to a server or read its contents
    console.log(event.target.files[0]);
    const file=event.target.files[0]
    const formdata=new FormData()
    formdata.append('file',file)
    try {
     const res=await fetch('https://localhost:7126/api/PDF',{
      method:'POST',
      body: formdata
     }) 

      if (res.ok) {
        alert('File uploaded successfully');
      } else {
        alert('File upload failed');
      }
    } catch (error) {
     console.error(error) 
    }
    
  };
  return (
    <Box>
      <input
        type="file"
        ref={fileInputRef}
        accept=".pdf"
        onChange={handleFileChange}
        style={{ display: "none" }}
      />
      <IconButton onClick={handleButtonClick} icon={<MdUploadFile size="15" />}></IconButton>
    </Box>
  );
}
