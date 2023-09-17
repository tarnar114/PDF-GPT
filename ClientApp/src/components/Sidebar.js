import { Flex, Skeleton, useColorModeValue } from "@chakra-ui/react";
import React, { useCallback } from "react";
import Toggle from "../theming/Toggle";
import Upload from "./Upload";
import { useDropzone } from "react-dropzone";
import FileCard from "./FileCard";

export default function Sidebar({ file, setFile, loading, setLoading }) {
  return (
    // <Skeleton isLoaded={loading}>
    <Flex
      h="100vh"
      w="30vh"
      bg="#171923"
      flexDirection="column"
      justifyContent="space-between"
    >
      <Flex justify="center" mt="5">
        {file ? <FileCard fileName={file.name} /> : null}
      </Flex>
      <Flex w="100%" marginBottom="5" justify="space-around">
        <Toggle />
        <Upload fileInfo={file} setFile={setFile} Uploading={setLoading} />
      </Flex>
    </Flex>
    // </Skeleton>
  );
}
