import { Box, Flex } from "@chakra-ui/react";
import React, { useState, useEffect } from "react";
import { Route, Routes } from "react-router-dom";
import Sidebar from "./components/Sidebar";
import Toggle from "./theming/Toggle";
import Chatbox from "./components/Chatbox";
import APIKeyModal from "./components/Modal";

function App() {
  const [kernelInit, setKernelInit] = useState(true);
  const [file, setFile] = useState(null);
  const [loading, setLoading] = useState(false);
  useEffect(() => {
    const fetchData = async () => {
      setLoading(true);
      fetch("https://localhost:7126/api/PDF/memCheck")
        .then((res) => res.json())
        .then((data) => {
          console.log(data);
          if (data === true) {
            setKernelInit(true);
          } else {
            setKernelInit(false);
          }
        });
      setLoading(false);
    };
    fetchData();
    console.log("api useeffect ran");
  }, []);
  useEffect(()=>{
    console.log("kernel init changed")
  },[kernelInit])
  return (
    <Box>
      <APIKeyModal init={!kernelInit} setKernelInit={setKernelInit} />
      <Flex>
        <Sidebar
          file={file}
          setFile={setFile}
          loading={loading}
          setLoading={setLoading}
        />
        <Box w="100%">
          <Chatbox file={file} />
        </Box>
      </Flex>
    </Box>
  );
}
export default App;
