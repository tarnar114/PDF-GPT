import {
  useDisclosure,
  Modal,
  ModalBody,
  ModalHeader,
  ModalContent,
  ModalOverlay,
  Input,
  ModalFooter,
  Button,
} from "@chakra-ui/react";
import React, { useEffect, useRef } from "react";

export default function APIKeyModal({init,setKernelInit}) {
    const inputRef=useRef(null)
  const { isOpen, onOpen, onClose } = useDisclosure();
  // useEffect(()=>{
  //   console.log(init)
  // },[])
  const Submit = async() => {
    const userKey=inputRef.current.value
    var userData={Key:userKey}
    
    try {
      const res=await fetch("https://localhost:7126/api/PDF/InitKernel",{
        method:"POST",
        body:JSON.stringify(userData),
        mode:"cors",
        headers:{
          "Content-Type":"application/json"
        }
      })
      const answer=await res.json(); 
      setKernelInit(true)
    } catch (err) {
      console.log(err)
    }
  };
  return (
    <Modal isCentered isOpen={init}>
      <ModalOverlay bg="blackAlpha.300" backdropFilter="blur(10px) " />
      <ModalContent>
        <ModalHeader>Register OpenAI Key</ModalHeader>
        <ModalBody>
          <Input placeholder="API key" ref={inputRef}  />
        </ModalBody>
        <ModalFooter>
          <Button type="submit" onClick={Submit}>Submit</Button>
        </ModalFooter>
      </ModalContent>
    </Modal>
  );
}
