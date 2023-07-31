import { Flex, useColorModeValue } from "@chakra-ui/react";
import React, { useCallback } from "react";
import Toggle from "../theming/Toggle";
import Upload from "./Upload";
import { useDropzone } from "react-dropzone";
import FileCard from "./FileCard";

export default function Sidebar({file,setFile,setFileLoading}) {
  const bg = useColorModeValue("#FEEBC8", "#171923");


  return (
    <Flex h="100vh" w="30vh" bg={bg} flexDirection="column" justifyContent="space-between" >
        <Flex justify="center" mt="5">
          {
            file
            ?(<FileCard/>)
            :null

          }
        </Flex>
        <Flex w="100%" marginBottom="5" justify="space-around" >
          <Toggle />
          <Upload fileInfo={file} setFile={setFile} Uploading={setFileLoading}/>
        </Flex>
    </Flex>
  );
}
