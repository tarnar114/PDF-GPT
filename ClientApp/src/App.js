import { Box, ChakraBaseProvider, Flex, Text } from "@chakra-ui/react";
import React, { Component ,useEffect,useRef,useState} from "react";
import { Route, Routes } from "react-router-dom";
import Sidebar from "./components/Sidebar";
import Toggle from "./theming/Toggle";
import Chatbox from "./components/Chatbox";

function App() {
  // static displayName = App.name;
  
    const [file,setFile]=useState(null)
    const [fileUploading,setFileUploading]=useState(false)
    return (
      <Box>
        <Flex>
          <Sidebar file={file} setFile={setFile} setFileLoading={setFileUploading} />
          <Box w="100%">
            <Chatbox  file={file} />
          </Box>
        </Flex>
      </Box>
    );
  }
export default App;
